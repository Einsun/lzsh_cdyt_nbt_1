<template>
  <div class="card">
    <div class="header flex-center-cross" v-if="showHeader">
      <div class="icon-wrapper">
        <sn-icon name="title-icon" :size="16" path="homepage/card" :rotate="true"></sn-icon>
      </div>
      <div class="title overflow">
        {{title}}
        <slot name="title"></slot>
      </div>
      <div class="trim-strip-wrapper">
          <sn-icon name="inclined-line" path="homepage/card" v-for="item of number" :key="item" margin="0" :size="12"></sn-icon>
      </div>
    </div>
    <div class="content-wrapper">
      <slot></slot>
    </div>
  </div>
</template>

<script>
export default {
  name: 'index',
  data () {
    return {
      timer: {},
      number: 1
    }
  },
  props: {
    showHeader: {
      type: Boolean,
      default () {
        return true
      }
    },
    title: String,
    decorationNumber: {
      type: Number,
      default () {
        return 20
      }
    }
  },
  watch: {
    title (val) {
      this.number = 1
    }
  },
  mounted () {
    this.timer = setInterval(() => {
      this.number = this.number === this.decorationNumber ? 1 : this.number + 1;
    }, 1000)
  },
  beforeDestroy () {
    if (this.timer) {
      clearInterval(this.timer)
    }
  }
}
</script>

<style lang="scss" scoped>
.card{
  width: 100%;
  height: 100%;
  background:linear-gradient(180deg,rgba(15,29,55,0.5) 50%,rgba(23,74,127,1) 100%);
  box-shadow:0px 1px 10px 0px rgba(50,123,225,0.5);
  border-radius:4px;
  border:1px solid rgba(50,123,225,1);
  padding: 15px 14px;
  .header{
    background:linear-gradient(270deg,rgba(14,29,52,0) 0%,rgba(16,59,101,0.8) 100%);
    height: 35px;
    font-size:14px;
    font-family:PingFangSC-Medium,PingFangSC;
    font-weight:500;
    color:rgba(46,192,243,1);
    .title {
      max-width: 10em;
    }
    .icon-wrapper{
      padding-left: 10px;
    }
    .trim-strip-wrapper{
      display: flex;
      margin-left: auto;
      margin-right: 6px;
    }
  }
  .content-wrapper{
    height: calc(100% - 35px);
    overflow-y: scroll;
    -ms-overflow-style: none;
    &::-webkit-scrollbar {
      width: 0 !important
    }
  }
}
</style>
