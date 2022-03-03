import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { th } from "date-fns/locale";
import { makeAutoObservable, runInAction } from "mobx";
import { ChatComment } from "../models/comment";
import { store } from "./store";

export default class CommentStore
{
    comments: ChatComment[] = [];
    hubConnection: HubConnection | null = null;

    constructor(){
        makeAutoObservable(this);
    }

    createHubConnection = (activityId: string) =>{
        if(store.activityStore.selectedActivity){
            // connection ayarla
            this.hubConnection = new HubConnectionBuilder()
                .withUrl('http://localhost:5000/chat?activityId=' + activityId, {
                    accessTokenFactory: () => store.userStore.user?.token!
                })
                // eğer connection koparsa clientı yeniden bağla
                .withAutomaticReconnect()
                .configureLogging(LogLevel.Information)
                .build();

            this.hubConnection.start().catch(error=> console.log('Error establishing the connection: ', error))
            // apideki metodu çağır
            this.hubConnection.on('LoadComments', (comments: ChatComment[])=> {
                runInAction(()=>{
                    comments.forEach(comment => {
                        comment.createdAt = new Date(comment.createdAt + 'Z');
                    })
                    this.comments = comments;
                });
            })
            
            this.hubConnection.on('RecieveComment', (comment: ChatComment) =>{
                runInAction(()=>{
                    comment.createdAt = new Date(comment.createdAt);
                    this.comments.unshift(comment);
                });
            })
        }
    }
    // clientın bağlantısını kes
    stopHubConnection = () => {
        this.hubConnection?.stop().catch(error=>console.log('Error stopping connection: ', error));
    }

    clearComments = () => {
        this.comments = [];
        this.stopHubConnection();
    }

    addComment = async (values: any) => {
        values.activityId = store.activityStore.selectedActivity?.id;
        try {
            // clientside da olan metodu kullan
            await this.hubConnection?.invoke('SendComment', values);
        } catch (error) {
            console.log(error);
        }
    }
}