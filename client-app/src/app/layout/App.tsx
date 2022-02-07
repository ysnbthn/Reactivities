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

  useEffect(() => {
    // response olarak ne geleceğini belirt
    axios.get<Activity[]>('http://localhost:5000/api/activities').then(response => {
      setActivities(response.data);
    })
  }, []);

  return (
    <Fragment>
      <NavBar />
      <Container style={{marginTop: '7em'}}>
        <ActivityDasboard activities={activities}/>
      </Container>
        
    </Fragment>
  );
}

export default App;
