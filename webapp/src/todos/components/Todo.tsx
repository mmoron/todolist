import React from 'react';
import { Todo as TodoItem } from '../models/Todo';

export const Todo = ({text, completed}: TodoItem) => {
    let style = {};
    if (completed) {
        style = {textDecoration: "line-through"};
    }
    return <div style={style}>* { text }</div>
}