import React, { Fragment, useEffect} from 'react';
import { Container} from 'semantic-ui-react';
import NavBar from './navbar';
import ActivityDasboard from '../../features/activities/dashboard/ActivityDasboard';
import LoadingComponent from './LoadingComponent';
import { useStore } from '../stores/store';
import { observer } from 'mobx-react-lite';

function App() {
  // storedan stateleri çek
  const {activityStore} = useStore();

  useEffect(() => {
    // storedan çek
      activityStore.loadActivities();
  }, [activityStore]); // dependency

  if(activityStore.loadingInitial){ return <LoadingComponent content='Loading app' />};

  return (
    <Fragment>
      <NavBar />
      <Container style={{marginTop: '7em'}}>
        <ActivityDasboard />
      </Container>   
    </Fragment>
  );
}
// observer değişimlerini takip etsin diye
export default observer(App);
