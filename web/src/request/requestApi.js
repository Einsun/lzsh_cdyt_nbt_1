import { createAPI } from './requestUtils'

const files = require.context('./modules', false, /\.js$/)
const modules = {}

files.keys().forEach(key => {
  modules[key.replace(/(\.\/|\.js)/g, '')] = files(key).default
})

const obj = {
  // login
  login (opts = {}) {
    const {
      data: {
        user = '',
        password = ''
      } = {}
    } = opts
    return createAPI(
      {
        url: '/UserInfo/Login',
        method: 'POST',
        headers: {
          'Content-Type': 'application/x-www-form-urlencoded'
        },
        data: {
          user,
          password
        }
      },
    );
  },
  getAlarmData (opts = {}) {
    return createAPI(
      Object.assign(
        {
          url: 'DashBoard/ProductLineSummary',
          method: 'GET',
          timeout: 90000,
        },
        opts
      )
    );
  },
  // device
  ...modules
}

export default obj
