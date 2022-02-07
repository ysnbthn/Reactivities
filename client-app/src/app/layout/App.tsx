import React, { Fragment, useEffect, useState } from 'react';
import { Container} from 'semantic-ui-react';
import { Activity } from '../models/activity';
import NavBar from './navbar';
import ActivityDasboard from '../../features/activities/dashboard/ActivityDasboard';
import {v4 as uuid} from 'uuid';
import agent from '../api/agent';
import LoadingComponent from './LoadingComponent';

function App() {
  // use state içine activity interface arrayi ekle
  // interface içinde activity modeli var
  const [activities, setActivities] = useState<Activity[]>([]);
  // activite seçimini ayarlamak için
  const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
  // edit listesi çıkartmak için
  const [editMode, setEditMode] = useState(false);
  // aradaki yükleme ekranı için
  const [loading, setLoading] = useState(true);
  // create edit için
  const [submitting, setSubmitting] = useState(false);


  useEffect(() => {
    // response olarak ne geleceğini belirt
    
    agent.Activities.list().then(response => {
      let activities: Activity[] = [];
      response.forEach(activity => {
        activity.date = activity.date.split('T')[0];
        activities.push(activity);
      })
      setActivities(activities);
      setLoading(false);
    })
  }, []);

function handleSelectActivity(id: string) {
  setSelectedActivity(activities.find(x=>x.id === id));
}

function handleCancelSelectActivity(){
  setSelectedActivity(undefined);
}

function handleFormOpen(id?: string) {
  id ? handleSelectActivity(id) : handleCancelSelectActivity();
  setEditMode(true);
}

function handleFormClose(){
  setEditMode(false);
}

function handleCreateOrEditActivity(activity: Activity){
  setSubmitting(true);
  if(activity.id) {
    agent.Activities.update(activity).then(()=>{
      setActivities([...activities, {...activity, id: uuid()}]);
      setSelectedActivity(activity);
      setEditMode(false);
      setSubmitting(false);
    })
  }else{
    activity.id = uuid();
    agent.Activities.create(activity).then(()=>{
      setActivities([...activities, activity]);
      setSelectedActivity(activity);
      setEditMode(false);
      setSubmitting(false);
    })
  }
}

function handleDeleteActivity(id:string){
  setSubmitting(true);
  agent.Activities.delete(id).then(() => {
    setActivities([...activities.filter(x=>x.id !== id)]);
    setSubmitting(false);
  })
  setActivities([...activities.filter(x=>x.id !== id)]);
}

  if(loading){ return <LoadingComponent content='Loading app' />};

  return (
    <Fragment>
      <NavBar openForm={handleFormOpen}/>
      <Container style={{marginTop: '7em'}}>
        <ActivityDasboard 
        activities={activities}
        selectedActivity={selectedActivity}
        selectActivity={handleSelectActivity}
        cancelSelectActivity={handleCancelSelectActivity}
        editMode={editMode}
        openForm={handleFormOpen}
        closeForm={handleFormClose}
        createOrEdit={handleCreateOrEditActivity}
        deleteActivity={handleDeleteActivity}
        submitting={submitting}
        />
      </Container>
        
    </Fragment>
  );
}

export default App;
