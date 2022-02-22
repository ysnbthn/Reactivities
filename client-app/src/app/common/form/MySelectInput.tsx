import { useField } from "formik";
import React from "react";
import { Form, Label, Select } from "semantic-ui-react";

interface Props {
    placeholder: string;
    name: string;
    options: any;
    label?: string;
}

export default function MySelectInput(props: Props){
    // helper manuel olarak inputu ayarlamanı sağlıyor
    const[field,meta, helpers] = useField(props.name);

    // !! değeri booleana çeviriyor (boş yada dolu durumuna göre)
    return(
        <Form.Field error={meta.touched && !!meta.error}>
            <label>{props.label}</label>
            <Select 
                clearable
                options={props.options}
                value={field.value || null}
                onChange={(e, d) => helpers.setValue(d.value)}
                onBlur={() => helpers.setTouched(true)}
                placeholder={props.placeholder}
            />
            {meta.touched && meta.error ?(
                <Label basic color='red'>{meta.error}</Label>
            ) : null }
        </Form.Field>
    )

}