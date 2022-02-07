import React from 'react';
import { Grid } from 'semantic-ui-react';
import { Activity } from '../../../app/models/activity';
import ActivityDetails from '../details/ActivityDetails';
import ActivityForm from '../form/ActivityForm';
import ActivityList from './ActivityList';

interface Props{
    activities : Activity[];
}

// semantic ui da grid 10 luk sistemden oluşuyor
// Props'u destructure ediyoruz props da kullanabilirsin
// bu şekilde birçok propu daha rahat childa gönderebiliriz
export default function ActivityDasboard({activities} : Props) {
    return(
        <Grid>
            <Grid.Column width='10'>
                <ActivityList activities={activities}/>
            </Grid.Column>
            <Grid.Column width='6'>
                {activities[0] && 
                <ActivityDetails activity={activities[0]} /> }
                <ActivityForm />
            </Grid.Column>
        </Grid>
    )
}
