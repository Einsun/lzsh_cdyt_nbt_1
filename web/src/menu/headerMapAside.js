const list = [
  { path: '/index',
    hiddenAside: true,
    aside: []
  },
  { path: '/alarm/all',
    hiddenAside: true,
    aside: [
      { path: '/alarm/all', title: '报警记录', icon: null },
    ]
  },
  {
    path: '/sys/watchkeeper',
    title: '系统管理',
    aside: [
      {
        title: '人员管理',
        path: '/sys/watchkeeper',
        icon: null
      },
    ]
  },
]

export default list
