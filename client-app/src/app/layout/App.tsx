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

  useEffect(() => {
    // response olarak ne geleceğini belirt
    axios.get<Activity[]>('http://localhost:5000/api/activities').then(response => {
      setActivities(response.data);
    })
  }, []);

function handleSelectedActivity(id: string) {
  setSelectedActivity(activities.find(x=>x.id === id));
}

function handleCancelSelectActivity(){
  setSelectedActivity(undefined);
}

  return (
    <Fragment>
      <NavBar />
      <Container style={{marginTop: '7em'}}>
        <ActivityDasboard 
        activities={activities}
        selectedActivity={selectedActivity}
        selectActivity={handleSelectedActivity}
        cancelSelectActivity={handleCancelSelectActivity}
        />
      </Container>
        
    </Fragment>
  );
}

export default App;
