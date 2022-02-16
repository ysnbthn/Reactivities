import React from 'react';
import { Message } from 'semantic-ui-react';

interface Props{
    errors: string[] | null 
}

export default function ValidationErrors({errors}: Props){

    return(
        
        <Message error>
            {errors && (     
                <Message.List>
                    {errors.map(function(err: any,i){
                        return <li key={i}>{err}</li>
                    })}
                </Message.List>
            )}
        </Message>
    )
}