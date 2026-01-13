import util from '@/libs/util.js'

export default {
  methods: {
    handleMenuSelect (index, indexPath) {
      console.log('index', index)
      const role = util.cookies.get('role')
      if (role !== 'Admin' && index === '/sys/watchkeeper') {
        this.$message.error('当前账户无操作权限')
        return;
      }
      if (/^d2-menu-empty-\d+$/.test(index) || index === undefined) {
        this.$message.warning(this.$store.state.sinuo.dashboard.VUE_APP_UNOPENED_PROMPT)
      } else if (/^https:\/\/|http:\/\//.test(index)) {
        util.open(index)
      } else {
        this.$router.push({
          path: index
        })
      }
    }
  }
}
