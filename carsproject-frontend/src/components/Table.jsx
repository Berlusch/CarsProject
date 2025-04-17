import React from 'react';
import './Table.css'; // Dodaj ovu liniju ako odvojiš CSS

const Table = ({ columns, data, onEdit, onRemove, onAdd, routeNames }) => {
  return (
    <div className="table-container">
      <button className="add-button" onClick={onAdd}>
        Add New
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
                <td>{item.Name}</td>
                <td>{item.Abrv}</td>
                <td>
  <button className="edit-button" onClick={() => onEdit(item.Id)}>
    <i className="fas fa-edit"></i> {/* Ikona za Edit */}
  </button>
</td>
<td>
  <button className="delete-button" onClick={() => onRemove(item.Id)}>
    <i className="fas fa-trash"></i> {/* Ikona za Delete */}
  </button>
</td>

              </tr>
            ))
          )}
        </tbody>
      </table>
    </div>
  );
};

export default Table;
