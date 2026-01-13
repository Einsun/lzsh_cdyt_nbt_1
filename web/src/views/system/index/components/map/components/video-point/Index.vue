<template>
  <sn-dialog class="ezui-video" :visible.sync="visible" :title="title" @close="onClose">
    <video-player  class="video-player vjs-custom-skin"
      ref="videoPlayer"
      :playsinline="true"
      :options="playerOptions"
    ></video-player>
  </sn-dialog>
</template>

<style lang="scss">
  .ezui-video{
    .video-player{
      width: 100%;
      height: auto;
    }
    object{
      margin:auto;
      display: block;
    }
  }
</style>

<script>
import * as EZUIKit from '@/plugin/ezuikit/ezuikit.js'
import 'videojs-contrib-hls'

export default {
  data: function () {
    return {
      innerVisible: false,
      title: null,
      src: null,
      player: null,
      playerOptions: {
        playbackRates: [0.7, 1.0, 1.5, 2.0],
        autoplay: false,
        muted: false,
        loop: false,
        preload: 'auto',
        language: 'zh-CN',
        aspectRatio: '16:9',
        fluid: true,
        sources: [{
          type: "application/x-mpegURL",
          src: ""
        }],
        poster: "",
        notSupportedMessage: '此视频暂无法播放，请稍后再试',
        controlBar: {
          timeDivider: true,
          durationDisplay: true,
          remainingTimeDisplay: false,
          fullscreenToggle: true
        }
      }
    };
  },
  props: {},
  computed: {
    visible: {
      get () {
        return this.innerVisible
      },
      set (val) {
        this.innerVisible = val
        if (!val) {
          this.src = null
          this.title = null
          if (this.player) {
            this.player.stop();
          }
        }
      },
    }
  },
  created () {
  },
  mounted () {
  },
  methods: {
    open (src, title) {
      this.visible = true;
      this.title = title;
      this.playerOptions.sources[0].src = src
    },
    onClose () {
      this.visible = false
    }
  }
};
</script>
