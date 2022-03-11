import React, { Fragment, useEffect } from 'react';
import { Container} from 'semantic-ui-react';
import NavBar from './navbar';
import ActivityDasboard from '../../features/activities/dashboard/ActivityDasboard';
import { observer } from 'mobx-react-lite';
import HomePage from '../../features/home/HomePage';
import { Route, Switch, useLocation } from 'react-router-dom';
import ActivityForm from '../../features/activities/form/ActivityForm';
import ActivityDetails from '../../features/activities/details/ActivityDetails';
import TestErrors from '../../features/errors/TestError';
import { ToastContainer } from 'react-toastify';
import NotFound from '../../features/errors/NotFound';
import ServerError from '../../features/errors/ServerError';
import { useStore } from '../stores/store';
import LoadingComponent from './LoadingComponent';
import ModalContainer from '../common/modals/ModalContainer';
import ProfilePage from '../../features/profiles/ProfilePage';
import PrivateRoute from './PrivateRoute';

function App() {
  const location = useLocation();
  const {commonStore, userStore} = useStore();

  // bu component yüklenince çalışır
  useEffect(()=>{
    if(commonStore.token){ 
      userStore.getUser().finally(()=> commonStore.setAppLoaded());
    }else{
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore]);

  if(!commonStore.appLoaded) return <LoadingComponent content='...Loading' />

  return (
    <Fragment>
      <ToastContainer position="bottom-right" hideProgressBar />
      <ModalContainer />
      <Route exact path='/' component={HomePage} />
      <Route 
        path={'/(.+)'}
        render={()=> (
          <Fragment>
            <NavBar />
            <Container style={{marginTop: '7em'}}>
              <Switch>
                <PrivateRoute exact path='/activities' component={ActivityDasboard} />
                <PrivateRoute path='/activities/:id' component={ActivityDetails} />
                <PrivateRoute key={location.key} path={['/createActivity', '/manage/:id']} component={ActivityForm} />
                <Route path='/profiles/:username' component={ProfilePage} />
                <PrivateRoute path='/errors' component={TestErrors} />
                <Route path='/server-error' component={ServerError} />
                <Route component={NotFound} />
              </Switch>
            </Container>  
          </Fragment>
        )}
      
      />
      
       
    </Fragment>
  );
}
// observer değişimlerini takip etsin diye
export default observer(App);
