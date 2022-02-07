import React, { Fragment, useEffect, useState } from 'react';
import { Container} from 'semantic-ui-react';
import axios from 'axios';
import { Activity } from '../models/activity';
import NavBar from './navbar';
import ActivityDasboard from '../../features/activities/dashboard/ActivityDasboard';

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
        />
      </Container>
        
    </Fragment>
  );
}

export default App;
