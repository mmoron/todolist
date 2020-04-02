import React from 'react';
import logo from './logo.svg';
import './App.css';
import { useQuery, gql } from '@apollo/client';
import { Todo } from './Todo';

const TODOS = gql`{
  todos {
    completed,
    text
  }
}
`;

function App() {
  const { loading, error, data } = useQuery(TODOS);

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error :(</p>;

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <div>
          TODO:
          {data.todos.map((todo: {text: string, completed: boolean}) => <Todo {...todo}/>)}
        </div>
      </header>
    </div>
  );
}

export default App;
