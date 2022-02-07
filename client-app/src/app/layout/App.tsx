import React, { Fragment, useEffect, useState } from 'react';
import { Container} from 'semantic-ui-react';
import axios from 'axios';
import { Activity } from '../models/activity';
import NavBar from './navbar';
import ActivityDasboard from '../../features/activities/dashboard/ActivityDasboard';
import {v4 as uuid} from 'uuid';

function App() {
  // use state içine activity interface arrayi ekle
  // interface içinde activity modeli var
  const [activities, setActivities] = useState<Activity[]>([]);
  // activite seçimini ayarlamak için
  const [selectedActivity, setSelectedActivity] = useState<Activity | undefined>(undefined);
  // edit listesi çıkartmak için
  const [editMode, setEditMode] = useState(false);

  useEffect(() => {
    // response olarak ne geleceğini belirt
    axios.get<Activity[]>('http://localhost:5000/api/activities').then(response => {
      setActivities(response.data);
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
  activity.id ? setActivities([...activities.filter(x=>x.id !== activity.id), activity])
  : setActivities([...activities, {...activity, id: uuid()}]);
  setEditMode(false);
  setSelectedActivity(activity);
}

function handleDeleteActivity(id:string){
  setActivities([...activities.filter(x=>x.id !== id)]);
}

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
        />
      </Container>
        
    </Fragment>
  );
}

export default App;
