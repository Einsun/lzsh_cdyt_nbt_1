import store from '@/store/index'
const filterMenu = [{
  type: 'input',
  label: '档案名称',
  key: 'file_name',
  value: '',
  style: {
    display: 'inline-flex'
  }
},
{
  type: 'input',
  label: '责任人',
  key: 'charge_person',
  value: '',
  style: {
    display: 'inline-flex'
  }
},
{
  type: 'date-picker',
  label: '创建时间',
  key: 'create_time',
  value: '',
  options: [
  ],
  style: {
    display: 'inline-flex'
  }
},
{
  type: 'select',
  label: '单位',
  key: 'unitID',
  value: '',
  options: [],
  style: {
    display: 'inline-flex'
  },
  props: {
    filterable: true
  }
},
{
  type: 'button',
  options: [
    {
      label: '搜索',
      type: 'submit',

    }, {
      label: '重置',
      type: 'reset',

    }
  ],
  style: {
    display: 'inline-flex'
  }
},

]
setTimeout(() => {
  store.watch((state, getters) => {
    return getters['sinuo/user/companyListOptions']
  }, () => {
    filterMenu.find(item => {
      return item.key === 'unitID'
    }).options = store.getters['sinuo/user/companyListOptions']
  }, {
    deep: true,
    immediate: true
  })
}, 0)
export { filterMenu }
