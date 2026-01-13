import layoutHeaderAside from '@/layout/header-aside'

// 由于懒加载页面太多的话会造成webpack热更新太慢，所以开发环境不使用懒加载，只有生产环境使用懒加载
const _import = require('@/libs/util.import.' + process.env.NODE_ENV)

const needAuth = true

/**
 * 在主框架内显示
 */
const frameIn = [
  {
    path: '/',
    redirect: { name: 'index' },
    component: layoutHeaderAside,
    children: [
      // 首页
      {
        path: 'index',
        name: 'index',
        meta: {
          auth: needAuth
        },
        component: _import('system/index')
      },
      // alarm
      {
        path: 'alarm',
        name: 'alarm',
        meta: {
          auth: needAuth
        },
        component: _import('system/alarm'),
        children: [
          // {
          //   path: 'handling', // 未处理
          //   component: _import('system/alarm/tabs/handling')
          // },
          // {
          //   path: 'handled', // 已处理
          //   component: _import('system/alarm/tabs/handled')
          // },
          {
            path: 'all', // 全部
            component: _import('system/alarm/tabs/all')
          },
        ]
      },
      // sys 系统管理
      {
        path: 'sys',
        name: 'sys',
        meta: {
          auth: needAuth
        },
        component: _import('system/sys'),
        children: [
          {
            path: 'watchkeeper', // 值班人员
            component: _import('system/sys/tabs/watchkeeper')
          },
        ]
      },
      // unit
      {
        path: 'unit',
        name: 'unit',
        meta: {
          auth: needAuth
        },
        component: _import('system/unit'),
      },
      // record
      {
        path: 'record',
        name: 'record',
        meta: {
          auth: needAuth
        },
        component: _import('system/record'),
        children: [
        ]
      },
      // device
      {
        path: 'device',
        name: 'device',
        meta: {
          auth: needAuth
        },
        component: _import('system/device')
      },
      // patrol
      {
        path: 'log',
        name: 'log',
        meta: {
          title: '前端日志',
          auth: needAuth
        },
        component: _import('system/log')
      },
      // 刷新页面 必须保留
      {
        path: 'refresh',
        name: 'refresh',
        hidden: needAuth,
        component: _import('system/function/refresh')
      },
      // 页面重定向 必须保留
      {
        path: 'redirect/:route*',
        name: 'redirect',
        hidden: needAuth,
        component: _import('system/function/redirect')
      }
    ]
  }
]

/**
 * 在主框架之外显示
 */
const frameOut = [
  // 登录
  {
    path: '/login',
    name: 'login',
    component: _import('system/login')
  }
]

/**
 * 错误页面
 */
const errorPage = [
  {
    path: '*',
    name: '404',
    component: _import('system/error/404')
  }
]

// 导出需要显示菜单的
export const frameInRoutes = frameIn

// 重新组织后导出
export default [
  ...frameIn,
  ...frameOut,
  ...errorPage
]
