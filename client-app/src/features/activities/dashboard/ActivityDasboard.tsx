import React from 'react';
import { Grid } from 'semantic-ui-react';
import { Activity } from '../../../app/models/activity';
import ActivityDetails from '../details/ActivityDetails';
import ActivityForm from '../form/ActivityForm';
import ActivityList from './ActivityList';

interface Props{
    activities : Activity[];
    selectedActivity : Activity | undefined;
    selectActivity : (id: string) => void;
    cancelSelectActivity : () => void;
}

// semantic ui da grid 10 luk sistemden oluşuyor
// Props'u destructure ediyoruz props da kullanabilirsin
// bu şekilde birçok propu daha rahat childa gönderebiliriz
export default function ActivityDasboard({activities, selectedActivity, selectActivity, cancelSelectActivity} : Props) {
    return(
        <Grid>
            <Grid.Column width='10'>
                <ActivityList activities={activities} selectActivity={selectActivity}/>
            </Grid.Column>
            <Grid.Column width='6'>
                {selectedActivity && 
                <ActivityDetails activity={selectedActivity} cancelSelectActivity={cancelSelectActivity} /> }
                <ActivityForm />
            </Grid.Column>
        </Grid>
    )
}
