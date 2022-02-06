import React, { useEffect, useState } from 'react';
import { Header, List } from 'semantic-ui-react';
import './App.css';
import axios from 'axios';

function App() {

  const [activities, setActivities] = useState([])

  useEffect(() => {
    axios.get('http://localhost:5000/api/activities').then(response => {
      setActivities(response.data);
    })
  }, [])

  return (
    <div >
      <Header as='h2' icon='users' content='Reactivities' />

        <List>
          {activities.map( (activity: any) => (
              <List.Item key={activity.id}>
                {activity.title}
              </List.Item>
            ))}
        </List>
    </div>
  );
}

export default App;
