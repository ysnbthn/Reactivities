import { format } from "date-fns";
import {makeAutoObservable, runInAction} from "mobx";
import agent from "../api/agent";
import { Activity } from "../models/activity";

export default class ActivityStore{
    activityRegistry = new Map<string, Activity>();
    selectedActivity: Activity | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = false;

    constructor(){
        //böyle hepsi otomatik oluşuyor
        makeAutoObservable(this)

        // böyle dersen hepsini tek tek belirtmen lazım
        // makeObservable(this, {
        //     title:observable,
        //     setTitle: action
        // })
    }

    get activitiesByDate(){
        return Array.from(this.activityRegistry.values()).sort((a,b)=> a.date!.getTime() - b.date!.getTime());
    }
    // activiteleri tarihe göre grupla
    get groupedActivities(){
        return Object.entries(
            this.activitiesByDate.reduce((activities, activity) => {
                const date = format(activity.date!, 'dd MMMM yyyy');
                activities[date] = activities[date] ? [...activities[date], activity] : [activity];
                return activities; 
            }, {} as {[key: string]: Activity[]})
        )
    }

    // action.bound kullanmak yerine arrow function yap
    loadActivities = async () => {
        this.loadingInitial = true;
        try {
            const activities = await agent.Activities.list();

                activities.forEach(activity => {
                    // mobx dışarıdan observable modify edemezsin 
                    // hatası vermesin diye böyle yaptık
                    this.setActivity(activity);
                  })
                  this.setLoadingInitial(false);
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    loadActivity = async (id: string) => {

        let activity = this.getActivity(id);
        if (activity) {
            // eğer activity varsa istek atmadan çek
            this.selectedActivity = activity;
            return activity;
        }else{
            this.loadingInitial = true;
            try {
                // yoksa yükleme iconunu göster o arada istek atıp çek
                activity = await agent.Activities.details(id);
                this.setActivity(activity);
                runInAction(()=>{
                    this.selectedActivity = activity;
                })
                this.setLoadingInitial(false);
                return activity;
            }catch(error) {
                console.log(error);
                this.setLoadingInitial(false);
            }
        }
    }

    private setActivity = (activity: Activity) => {
        activity.date = new Date(activity.date!);
        // bu class içindeki activities
        this.activityRegistry.set(activity.id, activity);
    }

    private getActivity = (id: string) => {
        return this.activityRegistry.get(id);
    }

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    

    createActivity = async (activity: Activity) => {
        this.loading = true;
        try {
            await agent.Activities.create(activity);
            runInAction(()=>{
                this.activityRegistry.set(activity.id, activity);
                this.selectedActivity = activity;
                this.editMode = false;
                this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(()=>{
                this.loading = false;
            })
        }   
    }

    updateActivity = async (activity: Activity) => {
        this.loading = true;
        try {
            await agent.Activities.update(activity);
            runInAction(()=>{
                this.activityRegistry.set(activity.id, activity);
                this.selectedActivity = activity;
                this.editMode = false;
                this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(()=>{
                this.loading = false;
            })
        }
    }

    deleteActivity = async (id:string) => {
        this.loading = true;
        try {
            await agent.Activities.delete(id);
            runInAction(()=>{
                this.activityRegistry.delete(id);
                this.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(()=>{
                this.loading = false;
            })
        }
    }
}