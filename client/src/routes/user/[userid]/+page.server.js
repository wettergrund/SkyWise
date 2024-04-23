/** 
 *  @param {object} param - param
 *  @param {slug} param.params - slug
 * 
 *  @typedef {object} slug - slug content
 *  @property {string} userid - userid
 */
export function load({ params }) {
  console.log(params)


  const result = params.userid;
 return {
    result
  };
}
