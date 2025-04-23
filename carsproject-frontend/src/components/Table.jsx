import React from 'react';
import './Table.css';
import { Link } from 'react-router-dom';

const Table = ({ columns, data, onAdd, routeNames, page=1}) => {
const hasData = Array.isArray(data) && data.length > 0;

console.log("Data:", data);
console.log("Page:", page);
console.log("hasData:", hasData);
  
  return (
    <div className="table-container">
      <table className="custom-table">
        <thead>
          <tr>
            <th colSpan={columns.length}className="add-button-container">
              <Link to={routeNames} style={{ textDecoration: 'none' }}>
                <button className="add-button-inside" onClick={onAdd}>
                  Add New
                </button>
              </Link>
            </th>
          </tr>
          <tr>
            {columns.map((col) => (
              <th key={col.accessor}>{col.header}</th>
            ))}
          </tr>
        </thead>
        <tbody>
  {hasData &&
    data.map((item, index) => (
      <tr key={index} className={index % 2 === 0 ? 'row-light' : 'row-white'}>
        {columns.map((col) => (
          <td key={col.accessor}>{item[col.accessor]}</td>
        ))}
      </tr>
    ))}

  {!hasData && page > 1 && (
    <tr>
      <td colSpan={columns.length} className="no-data-message">
        Oops! No more data to show. You can go back or add a new item.
      </td>
    </tr>
  )}
</tbody>

      </table>
    </div>
  );
};

export default Table;
