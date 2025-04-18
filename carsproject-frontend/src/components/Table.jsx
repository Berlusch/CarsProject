import React from 'react';
import './Table.css'; // Dodaj ovu liniju ako odvojiš CSS

const Table = ({ columns, data, onEdit, onRemove, onAdd, routeNames,entityName }) => {
  console.log('Columns:', columns);
  console.log('Data:', data);
  return (
    <div className="table-container">
      <button className="add-button" onClick={onAdd}>
        Add New {entityName}
      </button>

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
      <td colSpan={columns.length} className="no-data">
        No data available.
      </td>
    </tr>
  ) : (
    data.map((item, index) => (
      <tr key={index} className={index % 2 === 0 ? 'row-light' : 'row-white'}>
        {columns.map(col => (
          <td key={col.accessor}>{item[col.accessor]}</td>
        ))}
        
      </tr>
    ))
  )}
</tbody>
      </table>
    </div>
  );
};

export default Table;
