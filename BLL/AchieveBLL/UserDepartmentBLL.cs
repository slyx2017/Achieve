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
    public class UserDepartmentBLL
    {
        IUserDepartmentDAL dal = DALFactory.GetUserDepartmentDAL();


        /// <summary>
        /// 设置用户部门（单个用户）
        /// </summary>
        /// <param name="userId">用户主键</param>
        /// <param name="depIds">部门id，多个用逗号隔开</param>
        public bool SetDepartmentSingle(int userId, string depIds)
        {
            DataTable dt_user_dep_old = new DepartmentBLL().GetDepartmentByUserId(userId);  //用户之前拥有的部门
            List<UserDepartmentEntity> dep_addList = new List<UserDepartmentEntity>();     //需要插入部门的sql语句集合
            List<UserDepartmentEntity> dep_deleteList = new List<UserDepartmentEntity>();     //需要删除部门的sql语句集合

            string[] str_dep = depIds.Trim(',').Split(',');    //传过来用户勾选的部门（有去勾的也有新勾选的）

            UserDepartmentEntity userdepdelete = null;
            UserDepartmentEntity userdepadd = null;
            for (int i = 0; i < dt_user_dep_old.Rows.Count; i++)
            {
                //等于-1说明用户去掉勾选了某个部门 需要删除
                if (Array.IndexOf(str_dep, dt_user_dep_old.Rows[i]["departmentid"].ToString()) == -1)
                {
                    userdepdelete = new UserDepartmentEntity();
                    userdepdelete.DepartmentId = Convert.ToInt32(dt_user_dep_old.Rows[i]["departmentid"].ToString());
                    userdepdelete.UserId = userId;
                    dep_deleteList.Add(userdepdelete);
                }
            }

            if (!string.IsNullOrEmpty(depIds))
            {
                for (int j = 0; j < str_dep.Length; j++)
                {
                    //等于0那么原来的部门没有 是用户新勾选的
                    if (dt_user_dep_old.Select("departmentid = '" + str_dep[j] + "'").Length == 0)
                    {
                        userdepadd = new UserDepartmentEntity();
                        userdepadd.UserId = userId;
                        userdepadd.DepartmentId = Convert.ToInt32(str_dep[j]);
                        dep_addList.Add(userdepadd);
                    }
                }
            }
            if (dep_addList.Count == 0 && dep_deleteList.Count == 0)
                return true;
            else
                return dal.SetDepartmentSingle(dep_addList, dep_deleteList);
        }

        /// <summary>
        /// 设置用户部门（批量设置）
        /// </summary>
        /// <param name="userIds">用户主键，多个用逗号隔开</param>
        /// <param name="depIds">部门id，多个用逗号隔开</param>
        public bool SetDepartmentBatch(string userIds, string depIds)
        {
            List<UserDepartmentEntity> dep_addList = new List<UserDepartmentEntity>();     //需要插入部门的sql语句集合
            List<UserDepartmentEntity> dep_deleteList = new List<UserDepartmentEntity>();     //需要删除部门的sql语句集合
            string[] str_userid = userIds.Trim(',').Split(',');
            string[] str_dep = depIds.Trim(',').Split(',');

            UserDepartmentEntity userdepdelete = null;
            UserDepartmentEntity userdepadd = null;
            for (int i = 0; i < str_userid.Length; i++)
            {
                //批量设置先删除当前用户的所有部门
                userdepdelete = new UserDepartmentEntity();
                userdepdelete.UserId = Convert.ToInt32(str_userid[i]);
                dep_deleteList.Add(userdepdelete);

                if (!string.IsNullOrEmpty(depIds))
                {
                    //再添加设置的部门
                    for (int j = 0; j < str_dep.Length; j++)
                    {
                        userdepadd = new UserDepartmentEntity();
                        userdepadd.UserId = Convert.ToInt32(str_userid[i]);
                        userdepadd.DepartmentId = Convert.ToInt32(str_dep[j]);
                        dep_addList.Add(userdepadd);
                    }
                }
            }
            return dal.SetDepartmentBatch(dep_addList, dep_deleteList);
        }
    }
}
