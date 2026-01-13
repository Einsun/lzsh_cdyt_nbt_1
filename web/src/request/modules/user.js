import { createAPI } from '../requestUtils';

const user = {
  // 设备的增删改查
  add (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        user = '',
        password = '',
        role = '',
      } = {}
    } = opts
    return createAPI(
      {
        url: `UserInfo/AddUser`,
        method,
        header: { 'content-type': 'multipart/form-data' },
        [paramsKey]: {
          user,
          password,
          role,
        }
      }
    )
  },
  request (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = false,
        name = '',
        password = null,
        role = null,
      } = {}
    } = opts
    return createAPI(
      {
        url: `UserInfo${id ? '/' + id : ''}`,
        method,
        [paramsKey]: {
          name,
          password,
          role,
        }
      }
    )
  },
  updateChangePassword (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = '',
        oldpassword = '',
        measureConfig = null,
        levelConfig = null,
        timingConfig = null,
      } = {}
    } = opts
    return createAPI(
      {
        url: `UserInfo/ChangePassword`,
        method,
        [paramsKey]: {
          id,
          name,
          measureConfig,
          levelConfig,
          timingConfig,
        }
      }
    )
  },
}
export default user
