import React, { useState } from "react";
import { gql, useMutation } from "@apollo/client";

const ADD_TODO = gql`
  mutation AddTodo($input: AddTodoInput!) {
    addTodo(input: $input) {
        todo {
            completed,
            text,
            id
        }
    }
  }
`;

export const AddTodo = () => {
    const [currentText, setText] = useState<string>();
    const [addTodo] = useMutation(ADD_TODO);

    const onAddClick = () => {
        addTodo({variables: {input: {text: currentText}}, refetchQueries:['getTodos']});
    }
    const onTextChanged = (event: any) => {
        setText(event.target.value);
    }

    return <div>
        <input type={'text'} value={currentText} onChange={onTextChanged}/>
        <button onClick={onAddClick}>Add</button>
    </div>;
}