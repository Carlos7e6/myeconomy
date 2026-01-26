import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Menu from "../../components/menu/menu";
import Savings from "../savings/savings";
import Main from "../main/main";
import FixedCost from '../fixedCost/fixedCost';

import "./App.css";

function App() {
  return (
    <Router>
      <div className="App">
        <Menu />
        <Routes>
          <Route path="/savings" element={<Savings />} />
          <Route path="/main" element={<Main />} />
          <Route path="/fixedCost" element={<FixedCost />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
