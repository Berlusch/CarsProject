import React, { useState } from 'react';
import './Table.css';
import { Link } from 'react-router-dom';

const Table = ({ columns, data, onAdd, routeNames, page = 1, defaultSortKey }) => {
  const hasData = Array.isArray(data) && data.length > 0;
  
  const [sortConfig, setSortConfig] = useState({
    key: defaultSortKey || (columns[0]?.accessor ?? ""),
    direction: "desc", // default descending
  });

  const handleSort = (key) => {
    let direction = "asc";
    if (sortConfig.key === key && sortConfig.direction === "asc") {
      direction = "desc";
    }
    setSortConfig({ key, direction });
  };

  const sortedData = [...data].sort((a, b) => {
    if (a[sortConfig.key] < b[sortConfig.key]) return sortConfig.direction === "asc" ? -1 : 1;
    if (a[sortConfig.key] > b[sortConfig.key]) return sortConfig.direction === "asc" ? 1 : -1;
    return 0;
  });
  

  return (
    <div className="table-container">
      <table className="custom-table">
        <thead>          
          <tr>
            <th colSpan={columns.length} className="add-button-container">
              <Link to={routeNames} style={{ textDecoration: 'none' }}>
                <button className="add-button-inside" onClick={onAdd}>
                  Add New
                </button>
              </Link>
            </th>
          </tr>          
          <tr>
            {columns.map((col) => (
              <th key={col.accessor} onClick={() => handleSort(col.accessor)} style={{ cursor: 'pointer' }}>
                {col.header}{" "}
                {sortConfig.key === col.accessor && (
                  <span>{sortConfig.direction === "asc" ? "↑" : "↓"}</span>
                )}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {hasData ? (
            sortedData.map((item, index) => (
              <tr key={index} className={index % 2 === 0 ? 'row-light' : 'row-white'}>
                {columns.map((col) => (
                  <td key={col.accessor}>{item[col.accessor]}</td>
                ))}
              </tr>
            ))
          ) : page > 1 ? (
            <tr>
              <td colSpan={columns.length} className="no-data-message">
                Oops! No more data to show. You can go back or add a new item.
              </td>
            </tr>
          ) : (
            <tr>
              <td colSpan={columns.length} className="no-data-message">
                No data available.
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default Table;