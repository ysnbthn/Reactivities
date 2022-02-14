import React, { Fragment } from 'react';
import { Container} from 'semantic-ui-react';
import NavBar from './navbar';
import ActivityDasboard from '../../features/activities/dashboard/ActivityDasboard';
import { observer } from 'mobx-react-lite';
import HomePage from '../../features/home/HomePage';
import { Route, useLocation } from 'react-router-dom';
import ActivityForm from '../../features/activities/form/ActivityForm';
import ActivityDetails from '../../features/activities/details/ActivityDetails';

function App() {
  const location = useLocation();

  return (
    <Fragment>
      <Route exact path='/' component={HomePage} />
      <Route 
        path={'/(.+)'}
        render={()=> (
          <Fragment>
            <NavBar />
            <Container style={{marginTop: '7em'}}>
              <Route exact path='/' component={HomePage} />
              <Route exact path='/activities' component={ActivityDasboard} />
              <Route path='/activities/:id' component={ActivityDetails} />
              <Route key={location.key} path={['/createActivity', '/manage/:id']} component={ActivityForm} />
            </Container>  
          </Fragment>
        )}
      
      />
      
       
    </Fragment>
  );
}
// observer değişimlerini takip etsin diye
export default observer(App);
