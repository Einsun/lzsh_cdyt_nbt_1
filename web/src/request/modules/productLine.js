import { createAPI } from '../requestUtils';
import request from '@/plugin/axios';
const productLine = {
  // 设备的增删改查
  line (opts = {}) {
    console.log(opts,999);
    
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = false,
        name = '',
        image = '',
        File = '',
        level = '',
        location = '',
        life = '',
        installTime = ''
      } = {}
    } = opts
    console.log(name,
      image,
      File,
      level,
      location,
      life,
      installTime);
      let data = new FormData()
      data.append("name",name)
      data.append("image",image)
      data.append("File",File)
      data.append("level",level)
      data.append("location",location)
      data.append("life",life)
      data.append("installTime",installTime)
    // return createAPI(
    //   {
    //     url: `ProductLine${id ? '/' + id : ''}`,
    //     method,
    //     headers: {
    //       // 'Content-Type': 'application/x-www-form-urlencoded'
    //       'Content-Type': 'multipart/form-data',
    //       'test': 11
    //     },
    //     // [paramsKey]: {
    //     //   name,
    //     //   image,
    //     //   File,
    //     //   level,
    //     //   location,
    //     //   life,
    //     //   installTime,
    //     // }
    //   }
      return request({
        url: `ProductLine${id ? '/' + id : ''}`,
        method,
        headers: {
          // 'Content-Type': 'application/x-www-form-urlencoded'
          'Content-Type': 'multipart/form-data'
        },
        [paramsKey]: data
      }
    )
  },
  updateLine (opts = {}) {
    const { method = 'GET' } = opts
    const paramsKey = method === 'GET' ? 'params' : 'data'
    const {
      [paramsKey]: {
        id = '',
        name = '',
        File = '',
        level = '',
        location = '',
        life = '',
        installTime = ''
      } = {}
    } = opts
    let data = new FormData()
      data.append("name",name)
      data.append("id",id)
      data.append("File",File)
      data.append("level",level)
      data.append("location",location)
      data.append("life",life)
      data.append("installTime",installTime)
    return request(
      {
        url: `ProductLine`,
        method,
        headers: {
          'Content-Type': 'multipart/form-data'
        },
        [paramsKey]: data
        // [paramsKey]: {
        //   id,
        //   name,
        //   File,
        //   level,
        //   location,
        //   life,
        //   installTime,
        // }
      }
    )
  },
}
export default productLine
