import React from 'react';

export function Todo({text, completed}: {text: string, completed: boolean}) {
    let style = {};
    if (completed) {
        style = {textDecoration: "line-through"};
    }
    return <div style={style}>* { text }</div>
}