shader_type canvas_item;

uniform bool	active =true;
void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	vec4 pre_color = texture(TEXTURE, UV);
	vec4 write_color = vec4(1,1,1,pre_color.a);
	vec4 new_color = pre_color;
	if(active){
		new_color = write_color;
	}
	 COLOR = new_color;
}

void light() {
	// Called for every pixel for every light affecting the CanvasItem.
}
