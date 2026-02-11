import React, { useState, useEffect } from "react";
import "./fixedCost.css";

const FixedCost = ({ session }) => {
  const [fixedCosts, setFixedCosts] = useState([]); // Estado para almacenar los datos
  const [loading, setLoading] = useState(true); // Estado para manejar el estado de carga
  const [error, setError] = useState(null); // Estado para manejar errores
  const [newFixedCost, setNewFixedCost] = useState({
    id: 0,
    expense: "",
    amount: "",
    frequency: "",
  }); // Estado para almacenar los datos del nuevo gasto

  // Función para manejar el envío de los datos
  const handleSubmit = (e) => {
    e.preventDefault();
    sendNewFixedCost();
  };

  const sendNewFixedCost = async () => {
    
    try {
      const response = await fetch(
        "https://localhost:7254/Economy/SaveFixedCost",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            'Authorization': `Bearer ${session?.access_token}`
          },
          body: JSON.stringify(newFixedCost),
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      const data = await response.json();
      setFixedCosts((prevCosts) => [...prevCosts, data]);
    } catch (err) {
      console.error(err.message); // Guardar mensaje de error
    }
  };

  const fetchFixedCosts = async () => {
    console.log("Mi JWT Token es:", session?.access_token);
    try {
      const response = await fetch(
        "https://localhost:7254/FixedCost",
        {
          method: "GET",
          headers: {
              'Authorization': `Bearer ${session.access_token}`
            },
        }
      );

      if (!response.ok) {
        throw new Error(`HTTP error! Status: ${response.status}`);
      }

      const data = await response.json();
      setFixedCosts(data); // Guardar datos en el estado
    } catch (err) {
      setError(err.message); // Guardar mensaje de error
    } finally {
      setLoading(false); // Cambiar estado de carga
    }
  };

  useEffect(() => {
    fetchFixedCosts();
  }, []); // Se ejecuta solo una vez al montar el componente

  if (loading)
    return (
      <div className="container">
        <p>Loading...</p>
      </div>
    ); // Mostrar mientras carga
  if (error)
    return (
      <div className="container">
        <p>Error: {error}</p>
      </div>
    ); // Mostrar mientras carga

  return (
    <div className="container FixedCost">
      <h1>Fixed Cost</h1>
      <p>Here you can manage your fixed expenses</p>
      <table>
        <thead>
          <tr>
            <th>Expense</th>
            <th>Amount</th>
            <th>Frequency</th>
            <th>Action</th>
          </tr>
        </thead>
        <tbody>
          {fixedCosts.map((fixedCost) => {
            return (
              <tr key={fixedCost.id}>
                <td>{fixedCost.expense}</td>
                <td>{fixedCost.amount}</td>
                <td>{fixedCost.frequency}</td>
                <td>
                  <button onClick={handleSubmit}>Delete</button>
                </td>
              </tr>
            );
          })}
          {/* Fila para ingresar nuevos gastos */}
          <tr>
            <td>
              <input
                type="text"
                value={newFixedCost.expense}
                onChange={(e) =>
                  setNewFixedCost({ ...newFixedCost, expense: e.target.value })
                }
                placeholder="Enter expense"
              />
            </td>
            <td>
              <input
                type="number"
                value={newFixedCost.amount}
                onChange={(e) =>
                  setNewFixedCost({ ...newFixedCost, amount: e.target.value })
                }
                placeholder="Enter amount"
              />
            </td>
            <td>
              <input
                type="number"
                value={newFixedCost.frequency}
                onChange={(e) =>
                  setNewFixedCost({ ...newFixedCost, frequency: e.target.value })
                }
                placeholder="Enter frequency"
              />
            </td>
            <td>
              <button onClick={handleSubmit}>Add Expense</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  );
};

export default FixedCost;
