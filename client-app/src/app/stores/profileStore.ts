
import { makeAutoObservable, reaction, runInAction } from "mobx";
import agent from "../api/agent";
import { Photo, Profile } from "../models/profile";
import { store } from "./store";

export default class ProfileStore{
    profile: Profile | null =  null;
    loadingProfile = false;
    uploading = false;
    loading = false;
    followings: Profile[]= [];
    // çok fazla loadingi kullandığımız için buna özel ayrı verdik
    loadingFollowings = false;
    activeTab = 0;

    constructor(){
        makeAutoObservable(this);
        // reaction ile aktif olan tabı al ve fonksiyonu ona göre çağır
        reaction(
            () => this.activeTab,
            activeTab => {
                if(activeTab === 3 || activeTab === 4){
                    const predicate = activeTab === 3 ? 'followers' : 'following';
                    this.loadFollowings(predicate);
                }else{
                    // tab değişince followingleri sıfırla
                    this.followings = [];
                }
            }
        )
    }
    // aktif olanı değiş
    setActiveTab = (activeTab: any) => {
        this.activeTab = activeTab;
    }

    get isCurrentUser(){
        if(store.userStore.user && this.profile){
            return store.userStore.user.username === this.profile.username;
        }
        return false;
    }

    loadProfile = async (username: string) =>{
        this.loadingProfile = true;
        try{
            const profile = await agent.Profiles.get(username);
            runInAction(()=>{
                this.profile = profile;
                this.loadingProfile = false;
            })
        }catch (error){
            console.log(error);
            runInAction(() => this.loadingProfile = false);
        }
    }

    uploadPhoto = async (file:Blob) => {
        this.uploading = true;
        try {
            const response = await agent.Profiles.uploadPhoto(file);
            const photo = response.data;
            runInAction(()=>{
                if(this.profile){
                    this.profile.photos?.push(photo);
                    if(photo.isMain && store.userStore.user){
                        store.userStore.setImage(photo.url);
                        this.profile.image = photo.url;
                    }
                }
                this.uploading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => this.uploading = false);
        }
    }

    setMainPhoto = async (photo: Photo) =>{
        this.loading = true;
        try {
            await agent.Profiles.setMainPhoto(photo.id);
            store.userStore.setImage(photo.url);
            runInAction(() => {
                if(this.profile && this.profile.photos){
                    // var olan main photonun statusunu değişir
                    this.profile.photos.find(p=>p.isMain)!.isMain = false;
                    this.profile.photos.find(p=>p.id === photo.id)!.isMain = true;
                    this.profile.image = photo.url;
                    this.loading = false;
                }
            })
        } catch (error) {
            runInAction(()=> this.loading = false);
            console.log(error);
        }
    }

    deletePhoto = async (photo: Photo) => {
        this.loading = true;
        try {
            await agent.Profiles.deletePhoto(photo.id);
            runInAction(()=> {
                if(this.profile){
                    this.profile.photos = this.profile.photos?.filter(p =>p.id !== photo.id);
                    this.loading = false;
                }
            });
        } catch (error) {
            runInAction(()=> this.loading = false);
            console.log(error);
        }
    }

    updateProfile = async (profile: Partial<Profile>) => {
        this.loading = true;
        try {
            await agent.Profiles.updateProfile(profile);
            runInAction(() => {
            if (profile.displayName && profile.displayName !== store.userStore.user?.displayName) {
                store.userStore.setDisplayName(profile.displayName);
            }
            this.profile = {...this.profile, ...profile as Profile};
            this.loading = false;
        })
        } catch (error) {
            console.log(error);
            runInAction(() => this.loading = false);
            }
        }
    
    updateFollowing = async (username: string, following: boolean) => {
        this.loading = true;
        try {
            // API endpointine post at
            await agent.Profiles.updateFollowing(username);
            // activity içindeki follower counterları güncelle
            store.activityStore.updateAttendeeFollowing(username);
            // profile içindeki follower counterları güncelle
            runInAction(()=> {
                                                                                                // \/ kendi profilinde başkasını takip eder yada çıkarırsan diye
                if(this.profile && this.profile.username !== store.userStore.user?.username && this.profile.username === username){
                    following ? this.profile.followersCount++ : this.profile.followersCount--;
                    this.profile.following = !this.profile.following;
                }
                // kendi profilinde başkasını takip eder yada çıkarırsan diye
                if(this.profile && this.profile.username === store.userStore.user?.username){
                    following ? this.profile.followingCount++ : this.profile.followingCount--;
                }
                this.followings.forEach(profile=> {
                    if(profile.username === username){
                        profile.following ? profile.followersCount-- : profile.followersCount++;
                        profile.following = !profile.following;
                    }
                })
                this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(()=> this.loading = false);
        }
    }

    loadFollowings = async(predicate:string) => {
        this.loadingFollowings = true;
        try {
            // API'a istek at gelen datayı buradaki followings arrayine at
            const followings = await agent.Profiles.listFollowings(this.profile!.username, predicate);
            runInAction(()=>{
                this.followings = followings;
                this.loadingFollowings = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(()=> this.loadingFollowings = false);
        }
    }
}