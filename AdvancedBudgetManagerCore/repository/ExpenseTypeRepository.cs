using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.exception;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace AdvancedBudgetManagerCore.repository {
    public class ExpenseTypeRepository : ICrudRepository<ExpenseType, long> {
        private IDatabaseConnection dbConnection;
        private string sqlStatementGetExpenseTypes = "SELECT categoryID, categoryName FROM expense_types ORDER BY categoryName";

        public ExpenseTypeRepository(IDatabaseConnection dbConnection) {
            this.dbConnection = dbConnection;
        }

        public IEnumerable<ExpenseType> GetAll() {
            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getExpenseTypesCommand = new MySqlCommand(sqlStatementGetExpenseTypes, conn);
                    conn.Open();

                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getExpenseTypesCommand);
                    DataTable retrievedExpenseTypes = new DataTable();
                    dataAdapter.Fill(retrievedExpenseTypes);

                    List<ExpenseType> expenseTypes = new List<ExpenseType>();
                    foreach (DataRow expenseTypeRow in retrievedExpenseTypes.Rows) {
                        long categoryId = -1;
                        string categoryName = string.Empty;

                        long.TryParse(expenseTypeRow.ItemArray[0].ToString(), out categoryId);
                        categoryName = expenseTypeRow.ItemArray[1].ToString();

                        ExpenseType expenseType = new ExpenseType(categoryId, categoryName);
                        expenseTypes.Add(expenseType);
                    }

                    return expenseTypes;

                } catch (MySqlException ex) {
                    int errorCode = ex.Number;
                    String message;

                    if (errorCode == 1042) {
                        message = "Unable to connect to the database! Please check the connection and try again.";
                    } else {
                        message = "An error occurred while retrieving data! Please try again.";
                    }

                    throw new AdvancedBudgetManagerException(ExceptionCategory.Persistence, message, ex);
                }
            }
        }

        public ExpenseType GetById(long id) {
            throw new NotImplementedException();
        }

        public ExpenseType Insert(ExpenseType entity) {
            throw new NotImplementedException();
        }

        public ExpenseType Update(ExpenseType entity) {
            throw new NotImplementedException();
        }
        public bool Delete(long id) {
            throw new NotImplementedException();
        }
    }
}
