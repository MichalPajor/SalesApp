using SalesApp.Models;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace SalesApp.SQLite
{
    public class SQLiteHelper
    {
        SQLiteAsyncConnection db;
        public SQLiteHelper(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<Units>().Wait();
            db.CreateTableAsync<TaxRates>().Wait();
            db.CreateTableAsync<Goods>().Wait();
            db.CreateTableAsync<Sales>().Wait();
            db.CreateTableAsync<AppSettings>().Wait();
            db.CreateTableAsync<Contractor>().Wait();
        }

        //ZAPYTANIA

        //Settings
        public Task<int> InsertAppSetting(AppSettings setting)
        {
            return db.InsertAsync(setting);
        }
        public Task<int> UpdateAppSetting(AppSettings setting)
        {
            return db.UpdateAsync(setting);
        }
        public Task<AppSettings> ReadAppSetting(string name)
        {
            return db.Table<AppSettings>().Where(i => i.Name.Equals(name)).FirstOrDefaultAsync();
        }

        //Units
        public Task<int> InsertUnit(Units unit)
        {
            return db.InsertAsync(unit);
        }
        public Task<List<Units>> ReadAllUnits()
        {
            return db.Table<Units>().OrderBy(x => x.Name).ToListAsync();
        }
        public Task<Units> ReadUnitbyId(int id)
        {
            return db.Table<Units>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> DeleteUnit(Units unit)
        {
            return db.DeleteAsync(unit);
        }

        //TaxRates
        public Task<int> InsertTaxRate(TaxRates tax)
        {
            return db.InsertAsync(tax);
        }
        public Task<List<TaxRates>> ReadAllTaxRates()
        {
            return db.Table<TaxRates>().ToListAsync();
        }
        public Task<TaxRates> ReadTaxRatebyId(int id)
        {
            return db.Table<TaxRates>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }
        public Task<int> UpdateTaxRate(TaxRates tax)
        {
            return db.UpdateAsync(tax);
        }
        public Task<List<TaxRates>> ReadAllTaxesWithChildren()
        {
            return db.GetAllWithChildrenAsync<TaxRates>();
        }

        //Goods
        public Task<int> InsertGoods(Goods good)
        {
            return db.InsertAsync(good);
        }
        public Task UpdateGoodsWithChildren(Goods good)
        {
            return db.UpdateWithChildrenAsync(good);
        }
        public Task<List<Goods>> ReadAllGoodsWithChildren()
        {
            return db.GetAllWithChildrenAsync<Goods>();
        }
        public Task<List<Goods>> ReadAllGoods()
        {
            return db.Table<Goods>().OrderBy(x => x.Name).ToListAsync();
        }
        public Task<int> DeleteGood(Goods good)
        {
            return db.DeleteAsync(good);
        }
        public Task<int> UpdateGood(Goods good)
        {
            return db.UpdateAsync(good);
        }
        public Task<Goods> ReadGoodByName(string name)
        {
            return db.Table<Goods>().Where(i => i.Name.Equals(name)).FirstOrDefaultAsync();
        }
        public Task<Goods> ReadGoodyUnitId(int id)
        {
            return db.Table<Goods>().Where(i => i.UnitsId == id).FirstOrDefaultAsync();
        }

        //Sales
        public Task<int> InsertSales(Sales sale)
        {
            return db.InsertAsync(sale);
        }
        public Task<List<Sales>> ReadAllSalesByDate(string date)
        {
            DateTime dateFrom = DateTime.Parse(date);
            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            DateTime dateTo = DateTime.Parse(date);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
            return db.Table<Sales>().Where(x => x.Date >= dateFrom && x.Date <= dateTo).OrderBy(x => x.ProductName).ToListAsync();
        }
        public Task<List<Sales>> ReadAllSalesByMonthAndYear(string currMonth)
        {         
            int month = int.Parse(currMonth.Substring(0, 2));
            int year = int.Parse(currMonth.Substring(3, 4));
            //DateTime dateHelper = new DateTime(year, month, 1, 0, 0, 0);
            DateTime dateFrom = new DateTime(year, month, 1, 0, 0, 0);
            DateTime dateTo  = new DateTime(year, month, DateTimeMethods.EndOfMonth(dateFrom).Day, 23, 59, 59);
          
            return db.Table<Sales>().Where(x => x.Date >= dateFrom && x.Date <= dateTo).OrderBy(x => x.ProductName).ToListAsync();
        }

        public Task<List<Sales>> ReadAllSalesBetweenDates(DateTime dateFrom, DateTime dateTo)
        {
            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);
            return db.Table<Sales>().Where(x => x.Date >= dateFrom && x.Date <= dateTo).ToListAsync();
        }

        //Contractors
        public Task<int> InsertContractor(Contractor contractor)
        {
            return db.InsertAsync(contractor);
        }
        public Task<List<Contractor>> ReadAllContractors()
        {
            return db.Table<Contractor>().OrderBy(x => x.Name).ToListAsync();
        }
        public Task<Contractor> ReadContractorByNIP(string nip)
        {
            return db.Table<Contractor>().Where(i => i.NIP.Equals(nip)).FirstOrDefaultAsync();
        }
        public Task<int> DeleteContractor(Contractor contractor)
        {
            return db.DeleteAsync(contractor);
        }
        public Task<int> UpdateContractor(Contractor contractor)
        {
            return db.UpdateAsync(contractor);
        }
    }
    public static class DateTimeMethods
    {
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0);
        }

        public static DateTime EndOfMonth(this DateTime date)
        {
            return date.StartOfMonth().AddMonths(1).AddSeconds(-1);
        }
    }
}
