﻿using AchieveCommon;
using AchieveDALFactory;
using AchieveEntity;
using AchieveInterfaceDAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AchieveBLL
{
    public class IconsBLL
    {
        IIconsDAL dal = DALFactory.GetIconsDAL();
        public IconsBLL()
        { }


        #region  Method

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Id)
        {
            return dal.Exists(Id);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsIconName(string iconName)
        {
            return dal.ExistsIconName(iconName);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(IconsEntity model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(IconsEntity model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {

            return dal.Delete(Id);
        }
        /// <summary>
        /// 批量删除一批数据
        /// </summary>
        public bool DeleteList(string Idlist)
        {
            return dal.DeleteList(Idlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public IconsEntity GetModel(int Id)
        {
            return dal.GetModel(Id);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 根据条件获取 
        /// </summary>
        public string GetAllIconsTree(string where)
        {
            DataTable dt = dal.GetList(where);

            string sb = "[";//[{\"id\":\"0\",\"text\":\"图标\",\"iconCls\":\"icon-application_view_icons\",\"children\": [
            foreach (DataRow dr in dt.Rows)
            {
                sb += "{\"id\":\"" + dr["IconCssInfo"] + "\",\"text\":\"" + dr["IconCssInfo"] + "\",\"iconCls\":\"" + dr["IconCssInfo"] + "\"},";//dr["IconName"]
            }
            sb = sb.Trim(",".ToCharArray());
            sb += "]"; //"]}]";
            return sb;
        }


        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataTable GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<IconsEntity> GetModelList(string strWhere)
        {
            DataTable dt = dal.GetList(strWhere);
            return DataTableToList(dt);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<IconsEntity> DataTableToList(DataTable dt)
        {
            List<IconsEntity> modelList = new List<IconsEntity>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                IconsEntity model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new IconsEntity();
                    if (dt.Rows[n]["Id"].ToString() != "")
                    {
                        model.Id = int.Parse(dt.Rows[n]["Id"].ToString());
                    }
                    model.IconName = dt.Rows[n]["IconName"].ToString();
                    model.IconCssInfo = dt.Rows[n]["IconCssInfo"].ToString();
                    if (dt.Rows[n]["CreateTime"].ToString() != "")
                    {
                        model.CreateTime = DateTime.Parse(dt.Rows[n]["CreateTime"].ToString());
                    }
                    model.CreateBy = dt.Rows[n]["CreateBy"].ToString();
                    if (dt.Rows[n]["UpdateTime"].ToString() != "")
                    {
                        model.UpdateTime = DateTime.Parse(dt.Rows[n]["UpdateTime"].ToString());
                    }
                    model.UpdateBy = dt.Rows[n]["UpdateBy"].ToString();
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columns">要取的列名（逗号分开）</param>
        /// <param name="order">排序</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="where">查询条件</param>
        /// <param name="totalCount">总记录数</param>
        public string GetPager(string tableName, string columns, string order, int pageSize, int pageIndex, string where, out int totalCount)
        {
            DataTable dt = SqlPagerHelper.GetPager(tableName, columns, order, pageSize, pageIndex, where, out totalCount);
            return JsonHelper.ToJson(dt);
        }
        #endregion
    }
}
