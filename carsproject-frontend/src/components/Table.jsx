import React from 'react';
import './Table.css'; 

const Table = ({ columns, data }) => {
  return (
    <table className="custom-table">
      <thead>
        <tr>
          {columns.map((col) => (
            <th key={col.accessor}>{col.header}</th>
          ))}
        </tr>
      </thead>
      <tbody>
        {data.length === 0 ? (
          <tr>
            <td colSpan={columns.length} style={{ textAlign: 'center' }}>
              Nema podataka za prikaz
            </td>
          </tr>
        ) : (
          data.map((row, rowIndex) => (
            <tr key={rowIndex}>
              {columns.map((col) => (
                <td key={col.accessor}>{row[col.accessor]}</td>
              ))}
            </tr>
          ))
        )}
      </tbody>
    </table>
  );
};

export default Table;
