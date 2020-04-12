import React from 'react';
import { Todo as TodoItem } from '../models/Todo';
import { gql, useMutation } from '@apollo/client';

const TOGGLE_TODO_COMPLETED = gql`
  mutation ToggleTodoCompleted($input: ToggleTodoCompletedInput!) {
    toggleTodoCompleted(input: $input) {
        todo {
            completed,
            text,
            id
        }
    }
  }
`;

export const Todo = ({id, text, completed}: TodoItem) => {
    const [toggleTodoCompleted] = useMutation(TOGGLE_TODO_COMPLETED);

    const onTodoClick = () => {
        toggleTodoCompleted({variables: {input: {id}}});
    }

    let style = {};
    if (completed) {
        style = {textDecoration: "line-through"};
    }
    return <div style={{...style, cursor: 'pointer'}} onClick={onTodoClick}>* { text }</div>
}