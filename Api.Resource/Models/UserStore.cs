using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Resource.Models
{
    public class UserStore
    {
        private static List<User> _users = new List<User>() {
            //某个用户
            new User {
                Id=13,
                Name="admin",
                Password="111111",
                //Role="admin",
                Email="admin@gmail.com",
                PhoneNumber="18800000000",
                Birthday = DateTime.Now,
                userRoles=new List<UserRole>
                { 
                     /*
                     用户拥有，所属的角色
                     */
                     new UserRole
                     {
                        Id=1,
                        UserId=3,
                        RoleId=1,
                        Role=new Role
                        {
                            Id=1,
                            Name="Admin"
                        },
                        /*
                         角色拥有的权限项
                         比如管理员：拥有新增，修改权限
                          测试：只有查看权限
                         */
                        Permissions=new List<RolePermission>
                        {
                            /*
                              某个权限项可以操作的资源信息 
                            */
                            new RolePermission
                            {
                                Id=1,
                                RoleId=1,
                                PermissionName=Permission.UserCreate,
                                permissionResources=new List<PermissionResource>
                                {
                                    new PermissionResource{ Id=1,RolePermissionId=1,Url="api/Identity/OtherInfo"},
                                    new PermissionResource{ Id=1,RolePermissionId=1,Url="api/Identity/oidc1"}
                                }
                            }
                        }
                     }
                },

            },
             new User {
                Id=2,
                Name="admin",
                Password="111111",
                Email="admin@gmail.com",
                PhoneNumber="18800000000",
                Birthday = DateTime.Now,
                userRoles=new List<UserRole>
                {
                     new UserRole
                     {
                        Id=1,
                        UserId=2,
                        RoleId=1,
                        Role=new Role
                        {
                            Id=1,
                            Name="Admin"
                        },
                        Permissions=new List<RolePermission>
                        {
                            new RolePermission
                            {
                                Id=1,
                                RoleId=1,
                                PermissionName=Permission.UserCreate,
                                permissionResources=new List<PermissionResource>
                                {
                                    new PermissionResource{ Id=1,RolePermissionId=1,Url="api/Identity/OtherInfo"},
                                    new PermissionResource{ Id=1,RolePermissionId=1,Url="api/Identity/oidc1"}
                                }
                            },
                            new RolePermission
                            {
                                Id=2,
                                RoleId=1,
                                PermissionName=Permission.UserDelete,
                                permissionResources=new List<PermissionResource>
                                {
                                    new PermissionResource{ Id=2,RolePermissionId=2,Url="api/Identity/del"},
                                    new PermissionResource{ Id=2,RolePermissionId=2,Url="api/Identity/all"}
                                }
                            }
                        }
                     },
                     new UserRole
                     {
                        Id=2,
                        UserId=2,
                        RoleId=2,
                        Role=new Role
                        {
                            Id=2,
                            Name="System"
                        },
                        Permissions=new List<RolePermission>
                        {
                            new RolePermission
                            {
                                Id=5,
                                RoleId=2,
                                PermissionName=Permission.UserRead,
                                permissionResources=new List<PermissionResource>
                                {
                                    new PermissionResource{ Id=1,RolePermissionId=5,Url="api/Identity/OtherInfo12"},
                                    new PermissionResource{ Id=1,RolePermissionId=5,Url="api/Identity/oidc112"}
                                }
                            }
                        }
                     }
                },

            },
        };


        public List<User> GetAll()
        {
            return _users;
        }

        public User Find(int id)
        {
            return _users.Find(_ => _.Id == id);
        }

        public User Find(string userName, string password)
        {
            return _users.FirstOrDefault(_ => _.Name == userName && _.Password == password);
        }

        public bool Exists(int id)
        {
            return _users.Any(_ => _.Id == id);
        }

        public void Add(User doc)
        {
            doc.Id = _users.Max(_ => _.Id) + 1;
            _users.Add(doc);
        }

        public void Update(int id, User doc)
        {
            var oldDoc = _users.Find(_ => _.Id == id);
            if (oldDoc != null)
            {
                oldDoc.Name = doc.Name;
                oldDoc.Email = doc.Email;
                oldDoc.Password = doc.Password;
            }
        }

        public void Remove(User doc)
        {
            if (doc != null)
            {
                _users.Remove(doc);
            }
        }

        //public bool CheckPermission(int userId, string permissionName, string url)
        //{
        //    var user = Find(userId);
        //    if (user == null) return false;

        //    return user.Permissions.Any(_ => _.PermissionName == permissionName && _.permissionAction.Any(_ => _.Url == url));

        //    //var any = user.Permissions.Any(a => a.PermissionName == permissionName);
        //    //if (any)
        //    //{
        //    //    var permissions = user.Permissions.Where(a => a.PermissionName == permissionName).First();
        //    //    any = permissions.permissionAction.Any(p => p.Url == url);
        //    //}
        //    //return any;

        //    //return user.Permissions.Any(p => permissionName.StartsWith(p.PermissionName));
        //}

        public bool CheckPermission(int userId, string url)
        {
            var user = Find(userId);
            if (user == null) return false;
            var IsAuthorize = false;

            /*
            1：先根据token中的角色，跟数据库中的角色对比
            2：获取角色对应的权限
            3：从权限中获取对应的资源跟当前访问的资源比较
            4：数据库中资源对应的权限，跟当前token中的权限是否匹配
            */

            //应该还要判断是否有所有权限


            //获取用户所属的角色
            var role = user.userRoles.Where(w => w.UserId == userId).FirstOrDefault();


            return user.userRoles.Any() //用户角色
                && user.userRoles.Any(u => u.Permissions.Any(_ => _.RoleId == u.RoleId)) //用户权限
                && user.userRoles.Any(
                    u => u.Permissions.Any(
                        p => p.permissionResources.Any(
                            _ => _.RolePermissionId == p.Id && _.Url == url)
                        )
                    );//用于权限可以操作的具体资源

            //if (role != null && role.Permissions.Any())
            //{
            //    /*
            //     用户访问的url，我这里是不知道这个url是属于什么权限
            //     只能逆推??设计有问题??
            //     注意：一个接口（资源）只会属于一个权限，比如接口A是新增，会在创建权限里面。不可能在删除权限找那个
            //     */
            //    //逆推，在用户的所有权限里面的，资源列表里面匹配用户访问的资源，匹配到了说明就可以操作该资源，不管是属于哪个角色的，
            //    if (role.Permissions.Any())
            //    {
            //        role.Permissions.ForEach(_ =>
            //        {
            //            if (IsAuthorize = _.permissionResources.Any(_ => _.Url == url)) return;
            //        });
            //    }
            //    //或者在接口上打个具体的权限标签，比如新增，删除
            //}
            //return IsAuthorize;
        }
    }
}
