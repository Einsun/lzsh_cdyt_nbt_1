import request from '@/plugin/axios'

export function AccountLogin ({ username, password }) {
  return request({
    url: '/UserInfo/Login',
    method: 'post',
    data: {
      user: username,
      password: password,
    }
  })
}
