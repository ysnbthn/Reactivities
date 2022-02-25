import React from 'react';
import { Message } from 'semantic-ui-react';

interface Props{
    errors: any;
}

export default function ValidationErrors({errors}: Props){

    return(
        
        <Message error>
            {errors && (     
                <Message.List>
                    {errors.map(function(err: any,i: any){
                        return <li key={i}>{err}</li>
                    })}
                </Message.List>
            )}
        </Message>
    )
}