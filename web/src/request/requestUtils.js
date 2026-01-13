import request from '@/plugin/axios';
import { omitBy } from 'lodash'
import store from '@/store/index'

function createAPI (conf, options = {}) {
  const paramsKey = (conf.method.toLowerCase() === 'get' ? 'params' : 'data')
  const needHandleObject = conf[paramsKey] || {}
  if (needHandleObject instanceof FormData) {
    // 目前先不进行修改
  } else {
    // 移除参数中的空值
    const handledObject = omitBy(needHandleObject, (val) => {
      const filterList = ['', undefined, null]
      return filterList.includes(val)
    })
    conf[paramsKey] = handledObject
  }
  return request(conf);
}

export { createAPI };
