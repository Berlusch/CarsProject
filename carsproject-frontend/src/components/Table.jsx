import React from 'react';
import './Table.css';
import { Link } from 'react-router-dom';
import { RouteNames } from '../common/constants';

const Table = ({ columns, data, onEdit, onRemove, onAdd, routeNames, entityName }) => {
  console.log('Columns:', columns);
  console.log('Data:', data);

  const hasData = data && data.length > 0;

  return (

    <div className="table-container">
      <Link to={RouteNames.CAR_MAKE_ADD}>
      <button className="add-button" onClick={onAdd}>
        Add New {entityName}
      </button>
      </Link>

      <table className="custom-table">
        {hasData ? (
          <>
            <thead>
              <tr>
                {columns.map((col) => (
                  <th key={col.accessor}>{col.header}</th>
                ))}
              </tr>
            </thead>
            <tbody>
              {data.map((item, index) => (
                <tr key={index} className={index % 2 === 0 ? 'row-light' : 'row-white'}>
                  {columns.map(col => (
                    <td key={col.accessor}>{item[col.accessor]}</td>
                  ))}
                </tr>
              ))}
            </tbody>
          </>
        ) : (
          <tbody>
            <tr>
              <td className="no-data-message" style={{ textAlign: "center", padding: "1rem" }}>
              Oops! No more data to show. You can go back or add a new car make.
              </td>
            </tr>
          </tbody>
        )}
      </table>
    </div>
  );
};

export default Table;
