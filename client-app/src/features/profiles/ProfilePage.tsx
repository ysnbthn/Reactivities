import { observer } from 'mobx-react-lite';
import React, { useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Grid } from 'semantic-ui-react';
import LoadingComponent from '../../app/layout/LoadingComponent';
import { useStore } from '../../app/stores/store';
import ProfileContent from './ProfileContent';
import ProfileHeader from './ProfileHeader';

// bir yerde store kullancaksan kullanacağın yeri oberver yapman gerekiyor
// eğer component da değişiklik olursa bilgileri yeniliyor
export default observer( function ProfilePage(){
    // algılamazsa soldakini böyle yap
    const {username} = useParams<{username: string}>();
    const {profileStore} = useStore();
    const {loadingProfile, loadProfile, profile} = profileStore;
    
    // load profile çağırabilmek için
    useEffect(()=>{
        loadProfile(username);
    }, [loadProfile, username]);

    if(loadingProfile) return <LoadingComponent content='Loading profile...' />

    return(
        <Grid>
            <Grid.Column width={16}>
                {profile && 
                <>
                 <ProfileHeader profile={profile} /> 
                 <ProfileContent profile={profile} />
                </>
               }
                
            </Grid.Column>
        </Grid>
    )
})