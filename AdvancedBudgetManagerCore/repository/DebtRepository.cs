using AdvancedBudgetManagerCore.model.entity;
using AdvancedBudgetManagerCore.utils.database;
using AdvancedBudgetManagerCore.utils.enums;
using AdvancedBudgetManagerCore.utils.exception;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace AdvancedBudgetManagerCore.repository {
    public class DebtRepository : IDebtRepository {
        private IDatabaseConnection dbConnection;
        private string sqlStatementGetDebtsByUserIdAndDateInterval = "SELECT debtID, user_ID, name, value, creditor_ID, date FROM debts WHERE user_ID = @userId AND date BETWEEN @startDate AND @endDate";

        public DebtRepository(IDatabaseConnection dbConnection) {
            this.dbConnection = dbConnection;
        }

        public bool Delete(long id) {
            throw new NotImplementedException();
        }

        public IEnumerable<Debt> GetAll() {
            throw new NotImplementedException();
        }

        public List<Debt> GetAllLikeName(long userId, string name) {
            throw new NotImplementedException();
        }

        public Debt GetById(long id) {
            throw new NotImplementedException();
        }

        public Debt GetByName(long userId, string name) {
            throw new NotImplementedException();
        }

        public List<Debt> GetByUserIdAndDateInterval(long userId, DateTime startDate, DateTime endDate) {
            using (MySqlConnection conn = (MySqlConnection)dbConnection.GetConnection()) {
                try {
                    MySqlCommand getDebtsByUserIdAndDateIntervalCommand = new MySqlCommand(sqlStatementGetDebtsByUserIdAndDateInterval, conn);
                    getDebtsByUserIdAndDateIntervalCommand.Parameters.AddWithValue("@userId", userId);
                    getDebtsByUserIdAndDateIntervalCommand.Parameters.AddWithValue("@startDate", startDate);
                    getDebtsByUserIdAndDateIntervalCommand.Parameters.AddWithValue("end", userId);
                    getDebtsByUserIdAndDateIntervalCommand.Parameters.AddWithValue("endDate", endDate);

                    conn.Open();
                    MySqlDataAdapter dataAdapter = new MySqlDataAdapter(getDebtsByUserIdAndDateIntervalCommand);
                    DataTable retrievedDebts = new DataTable();

                    dataAdapter.Fill(retrievedDebts);

                    List<Debt> debtsList = new List<Debt>();
                    foreach (DataRow debtRow in retrievedDebts.Rows) {
                        long debtId = -1;
                        long retrievedUserId = -1;
                        string name = string.Empty;
                        int value = -1;
                        long creditorId = -1;
                        DateTime date = DateTime.MinValue;

                        long.TryParse(debtRow.ItemArray[0].ToString(), out debtId);
                        long.TryParse(debtRow.ItemArray[1].ToString(), out retrievedUserId);
                        name = debtRow.ItemArray[2].ToString();
                        int.TryParse(debtRow.ItemArray[3].ToString(), out value);
                        long.TryParse(debtRow.ItemArray[4].ToString(), out creditorId);
                        date = DateTime.Parse(debtRow.ItemArray[5].ToString());
                        Debt debt = new Debt(debtId, retrievedUserId, name, value, creditorId, date);

                        debtsList.Add(debt);
                    }

                    return debtsList;

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

        public Debt Insert(Debt entity) {
            throw new NotImplementedException();
        }

        public Debt Update(Debt entity) {
            throw new NotImplementedException();
        }
    }
}
