import React from 'react';
import { Todo as TodoItem } from '../models/Todo';
import { Todo } from './Todo';

export const TodosList = ({todos}: {todos: TodoItem[]}) => {
    return <div>
        TODO:
        {todos.map(todo => <Todo {...todo}/>)}
    </div>;
}