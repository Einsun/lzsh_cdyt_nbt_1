export const fitFontSize = (maxWidth, $dom) => {
  const getStyle = () => {
    const style = window.getComputedStyle($dom)

    const { width, fontSize } = style

    return { width, fontSize }
  }

  const adjust = () => {
    const style = getStyle()

    for (let key in style) {
      style[key] = Number.parseInt(style[key])
    }
    let { width, fontSize } = style
    if (width > maxWidth) {
      $dom.style.fontSize = `${fontSize - 1}px`
      adjust()
    }
  }
  adjust()
}
