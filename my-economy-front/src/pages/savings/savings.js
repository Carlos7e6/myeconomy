import React from "react";
import "./savings.css";
import { useState } from "react";

const Savings = () => {
  const [monthly, setMonthly] = useState(false);
  const [yearly, setYearly] = useState(false);

  if (monthly) {
    return (
      <div className="container">
        <h2>Savings</h2>
        <button>Monthly</button>
        <button
          onClick={() => {
            setYearly(true);
            setMonthly(false);
          }}
        >
          Yearly
        </button>
        <p>Yearly</p>
      </div>
    );
  }

  if (yearly) {
    return (
      <div className="container">
        <h2>Savings</h2>
        <button
          onClick={() => {
            setMonthly(true);
            setYearly(false);
          }}
        >
          Monthly
        </button>
        <button>Yearly</button>
        <p>Monthly</p>
      </div>
    );
  }

  return (
    <div className="container">
      <h2>Savings</h2>
      <button onClick={() => setMonthly(true)}>Monthly</button>
      <button onClick={() => setYearly(true)}>Yearly</button>
      <p>Select how you want count your money</p>
    </div>
  );
};

export default Savings;
