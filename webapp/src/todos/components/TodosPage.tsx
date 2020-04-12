import React from "react"

import { TodosList } from "./TodosList"
import { AddTodo } from "./AddTodo"
import { useQuery } from "@apollo/client/react";
import { gql } from "@apollo/client";

const TODOS = gql`query getTodos {
    todos {
      completed,
      text
    }
  }
  `;

export const TodosPage = () => {
    const { loading, error, data } = useQuery(TODOS);

    if (loading) return <p>Loading...</p>;
    if (error) return <p>Error :(</p>;

    return <div>
        <TodosList todos={data.todos} />
        <AddTodo />
    </div>
}