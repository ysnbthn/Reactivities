import { makeAutoObservable, reaction } from "mobx";
import { ServerError } from "../models/serverError";

export default class CommonStore{
    error: ServerError | null = null;
    // yüklenir yüklenmez varsa user tokenı çek
    token: string  | null = window.localStorage.getItem('jwt');
    appLoaded = false;

    constructor(){
        makeAutoObservable(this);

        // token nulldan başka değere değişirse bu çağırılıyor
        reaction(
            ()=> this.token,
            token=>{
                if(token){
                    window.localStorage.setItem('jwt', token)
                }else{
                    window.localStorage.removeItem('jwt')
                }
            }
        )
    }

    setServerError = (error: ServerError) =>{
        this.error = error;
    }

    // gelen tokenı üsttekine eşitle
    setToken= (token: string | null)=>{
        this.token = token;
    }
    
    setAppLoaded = ()=>{
        this.appLoaded = true;
    }

}